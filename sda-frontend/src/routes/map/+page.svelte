<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, {
        FeatureGroup,
        Polyline,
        Popup,
        type LatLngExpression,
    } from "leaflet";
    import { onMount } from "svelte";
    export let data;
    import { env } from "$env/dynamic/public";

    // Extend the Marker class to hold which station it represents
    // This way we don't have to have a dictionary to translate them between layer id:station id
    export class StationMarker extends L.Marker {
        stationId: Number | undefined;
        constructor(latLng: LatLngExpression, options?: L.MarkerOptions) {
            super(latLng, options);
        }
        setStationId(id: Number): this {
            this.stationId = id;
            return this;
        }
    }

    let initPos: LatLngExpression = [
        Number(data.stations[0].coordinate_y),
        Number(data.stations[0].coordinate_x),
    ];
    let map: L.Map | undefined;
    let inpVal = "";
    let selectedStation: StationMarker | undefined = undefined;
    let visibleReturnStations: [station: StationMarker, nrOfTrips: Number][] =
        [];

    const markers: FeatureGroup = new FeatureGroup();
    const lines: FeatureGroup = new FeatureGroup();

    function createStationMarker(station: Station): StationMarker {
        return new StationMarker(
            [Number(station.coordinate_y), Number(station.coordinate_x)],
            {
                alt: station.station_name ?? "",
                title: station.station_name ?? "",
            },
        ).setStationId(station.id);
    }
    function createPopup(station: Station): Popup {
        return L.popup({}).setContent(
            "<p><a href=/stations/" +
                station.id +
                ">" +
                station.station_name +
                " " +
                station.id +
                "</a></p><p>" +
                station.station_address +
                "</p>",
        );
    }
    function createPolyLine(
        destinationMarker: StationMarker,
        returnStation: Returnstation,
    ): Polyline {
        // Find the the marker which belongs to the return station
        let returnStationMarker = markers
            .getLayers()
            .find(
                (layer) =>
                    (layer as StationMarker).stationId ===
                    returnStation.return_station,
            ) as StationMarker;

        return new Polyline(
            [
                [
                    destinationMarker.getLatLng().lat,
                    destinationMarker.getLatLng().lng,
                ],
                [
                    returnStationMarker.getLatLng().lat,
                    returnStationMarker.getLatLng().lng,
                ],
            ],
            { color: "red" },
        );
    }
    onMount(() => {
        data.stations.forEach((station) => {
            let marker = createStationMarker(station);
            let popup = createPopup(station);
            let tooltip = new L.Tooltip({
                content: "<strong>" + station.station_name + "<strong>",
                // direction: 'center',
                // offset: [-14, -30]
            });
            marker.bindTooltip(tooltip);
            marker.bindPopup(popup);
            markers.addLayer(marker);

            marker.on("popupopen", async (event) => {
                // Stop moving the view
                // Else it will be janky if we select another marker while one is selected
                map?.stop();
                console.log("Marker.on(popup_open)", event);
                let targetMarker = event.target as StationMarker;
                selectedStation = targetMarker;
                console.log("Marker: ", targetMarker);
                console.log("Marker station-id: ", targetMarker.stationId);

                // Fetch return stations
                let returnStationsResp = await fetch(
                    `${env.PUBLIC_BACKEND_API}/stations/${targetMarker.stationId}/depatures/returnstations`,
                );
                let returnStations: Returnstation[] =
                    await returnStationsResp.json();
                console.log("# Returnstations: ", returnStations.length);

                // Remove all markers except the one selected
                markers.eachLayer((layer) => {
                    let returnStation = returnStations.find(
                        (r) =>
                            r.return_station ===
                            (layer as StationMarker).stationId,
                    );
                    if (layer === targetMarker || returnStation) {
                        (layer as StationMarker).setOpacity(1);
                        visibleReturnStations.push([
                            layer as StationMarker,
                            Number(returnStation?.count),
                        ]);
                    } else {
                        (layer as StationMarker).setOpacity(0.4);
                    }
                });
                // Have to re-assign the array to 'force' reactivity on it
                visibleReturnStations = visibleReturnStations;
                // Draw lines between them
                map?.addLayer(lines);
                returnStations.forEach((returnStation) => {
                    createPolyLine(targetMarker, returnStation).addTo(lines);
                    // https://github.com/IvanSanchez/Leaflet.Polyline.SnakeAnim
                });
                map?.flyToBounds(lines.getBounds(), {
                    padding: [20, 20],
                    animate: true,
                });
                inpVal = "";
            });

            marker.on("popupclose", (event) => {
                console.log("Marker.on(popup_close)", event);
                selectedStation = undefined;
                visibleReturnStations = [];
                lines.clearLayers();
                map?.flyToBounds(markers.getBounds());
                markers.eachLayer((layer) => {
                    if (layer === event.target) {
                    } else {
                        (layer as StationMarker).setOpacity(1);
                    }
                });
                inpVal = "";
            });
        });
        map?.addLayer(markers);
        map?.flyToBounds(markers.getBounds());
        // DEV / TEST
        selectedStation = markers.getLayers()[0] as StationMarker;
        selectedStation.openPopup();
    });
    // Search/filter station function
    $: {
        console.log("search--val: " + inpVal);
        // To stop overriding opacity when clicking a filtered marker
        if (inpVal) {
            markers?.eachLayer((m) => {
                if (
                    !(m as L.Marker).options.title
                        ?.toLowerCase()
                        .startsWith(inpVal.toLowerCase())
                ) {
                    // Här är dom osynliga, men man kan nädå hovra
                    (m as StationMarker).setOpacity(0);
                } else {
                    (m as StationMarker).setOpacity(1);
                }
            });
        }
    }
    function sortByNrOfTrips(
        arr: [station: StationMarker, nrOfTrips: Number][],
    ): [StationMarker, Number][] {
        return arr.sort((a, b) => (a[1] > b[1] ? -1 : 1));
    }
</script>

<svelte:head>
    <title>Map</title>
    <meta name="description" content="Map" />
</svelte:head>

<div id="content">
    <div id="sidebar">
        <div id="search">
            <label for="stationSearch">Find station</label>
            <input
                id="stationSearch"
                bind:value={inpVal}
                placeholder="Search..."
            />
            <br style="clear:both;" />
        </div>
        <p>{inpVal}</p>

        <div id="stationList">
            {#if selectedStation}
                <ul>
                    <h2>
                        <a href="/stations/{selectedStation.stationId}">
                            {selectedStation.options.title}
                        </a>
                    </h2>
                    <p>
                        Has trips ending at {visibleReturnStations.length} stations
                    </p>
                    <table>
                        <th id="stationCol">Station</th>
                        <th id="returnsCol">Nr of trips</th>

                        {#each sortByNrOfTrips(visibleReturnStations) as returnStation}
                            <tr>
                                <td id="stationCol">
                                    <a href="/stations/{returnStation[0]
                                            .stationId}">
                                        {returnStation[0].options.title}
                                    </a>
                                </td>
                                <td id="returnsCol">
                                    {returnStation[1]}
                                </td>
                            </tr>
                        {/each}
                    </table>
                    <!-- {#each sortByNrOfTrips(visibleReturnStations) as returnStation}
                        <li>
                            <a href="/stations/{returnStation[0].stationId}"
                                >{returnStation[0].options.title}
                            </a>
                            {returnStation[1]} trips
                        </li>
                    {/each} -->
                </ul>
            {/if}
        </div>
    </div>
    <div id="map">
        <Map view={initPos} zoom={11} bind:map />
    </div>
</div>

<style>
    #content {
        height: 100%;
        width: 100%;
        overflow: hidden;
        display: flex;
    }
    #sidebar {
        min-width: 200px;
        width: 15%;
        overflow: auto;
        padding: 10px;
        padding-left: 20px;
        padding-right: 20px;
    }
    #search {
        display: flex;
        gap: 3px;
        flex-direction: column;
        justify-content: center;
    }
    #map {
        width: 100%;
        height: 100%;
    }
    ul {
        list-style-type: none;
        padding-left: 0px;
    }
    ul li {
        padding-bottom: 3px;
    }
    table {
        table-layout: fixed;
        width: 100%;
    }
    #returnsCol {
        text-align: right;
        text-overflow: ellipsis;
    }
    #stationCol {
        text-align: left;
        text-overflow: ellipsis;
        width: 60%;
        white-space: nowrap;
        overflow: hidden;
    }
</style>
