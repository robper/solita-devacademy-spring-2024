<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, {
        FeatureGroup,
        Polyline,
        Popup,
        type LatLngExpression,
    } from "leaflet";
    import { onMount } from "svelte";
    import { StationMarker } from "$lib/leafletTypes.js";
    import { env } from "$env/dynamic/public";
    import Search from "$lib/components/Search.svelte";
    import ReturnList from "$lib/components/ReturnList.svelte";
    import ResultList from "$lib/components/ResultList.svelte";

    export let data;

    let initPos: LatLngExpression = [
        Number(data.stations[0].coordinate_y),
        Number(data.stations[0].coordinate_x),
    ];
    let map: L.Map | undefined;
    let searchTerm = "";
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
                searchTerm = "";
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
                searchTerm = "";
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
        console.log("search--val: " + searchTerm);
        // To stop overriding opacity when clicking a filtered marker
        markers?.eachLayer((m) => {
            if (
                !(m as L.Marker).options.title
                    ?.toLowerCase()
                    .startsWith(searchTerm.toLowerCase())
            ) {
                if (searchTerm) {
                    lines.clearLayers();
                    //   selectedStation?.closePopup();
                    // Här är dom osynliga, men man kan nädå hovra
                    (m as StationMarker).setOpacity(0);
                }
            } else {
                (m as StationMarker).setOpacity(1);
                // (m as StationMarker).setIcon();// Sätta en annan icon?, om vi gör det gör funktion
            }
        });
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
            <Search bind:searchVar={searchTerm} placeholder="Search stations..." />
            <br style="clear:both;" />
        </div>

        <div id="stationList">
            {#if selectedStation}
                <ReturnList
                    returnStations={sortByNrOfTrips(visibleReturnStations)}
                    station={selectedStation}
                />
            {:else if searchTerm}
                <ResultList />
            {:else}
                <p>Nothing</p>
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
        min-width: 250px;
        width: 15%;
        overflow: auto;
        padding: 10px;
    }
    #search {
        width: 100%;
        display: flex;
    }
    #map {
        width: 100%;
        height: 100%;
    }
</style>
