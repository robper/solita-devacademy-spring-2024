<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, {
        FeatureGroup,
        Polyline,
        Popup,
        type FitBoundsOptions,
        type LatLngExpression,
    } from "leaflet";
    import { onMount } from "svelte";
    import { StationMarker } from "$lib/leafletTypes.js";
    import { env } from "$env/dynamic/public";
    import Search from "$lib/components/Search.svelte";
    import ReturnList from "$lib/components/ReturnList.svelte";
    import SearchResults from "$lib/components/SearchResults.svelte";

    export let data;

    const flyToOptions: FitBoundsOptions = {
        animate: true,
        duration: 0.5,
        easeLinearity: 0.5,
        padding: [20, 20],
    };
    let initPos: LatLngExpression = [
        Number(data.stations[0].coordinate_y),
        Number(data.stations[0].coordinate_x),
    ];
    let map: L.Map | undefined;
    let searchTerm = "";
    let selectedStation: StationMarker | undefined = undefined;
    let visibleReturnStations: [station: StationMarker, nrOfTrips: Number][] =
        [];

    const allMarkers: FeatureGroup<StationMarker> =
        new FeatureGroup<StationMarker>();
    let markers: FeatureGroup<StationMarker> =
        new FeatureGroup<StationMarker>();

    const lines: FeatureGroup = new FeatureGroup<Polyline>();

    function castFGToArr(featureGroup: FeatureGroup<StationMarker>): Station[] {
        let x = featureGroup.getLayers();
        return x as StationMarker[];
    }
    function createStationMarker(station: Station): StationMarker {
        return new StationMarker(
            [Number(station.coordinate_y), Number(station.coordinate_x)],
            {
                alt: station.station_name ?? "Unnamed Station",
                title: station.station_name ?? "Unnamed Station",
            },
        );
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
                    (layer as StationMarker).id ===
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
            {
                color: "#175a72",
                weight: 1,
                interactive: false, // Kan klicka på den och fokusera? orka
            },
        );
    }
    function sortByNrOfTrips(
        arr: [station: StationMarker, nrOfTrips: Number][],
    ): [StationMarker, Number][] {
        return arr.sort((a, b) => (a[1] > b[1] ? -1 : 1));
    }
    onMount(() => {
        data.stations.forEach((station) => {
            let marker = createStationMarker(station);
            marker.setStationId(station.id);
            marker.setStationName(station.station_name ?? "Unnamed Station");

            let popup = createPopup(station);
            let tooltip = new L.Tooltip({
                content: "<strong>" + station.station_name + "<strong>",
            });
            marker.bindTooltip(tooltip);
            marker.bindPopup(popup);
            markers.addLayer(marker);
            allMarkers.addLayer(marker);

            marker.on("popupopen", async (event) => {
                searchTerm = "";
                // Stop moving the view to prevent  janky if we select another marker while one is selected
                map?.stop();
                console.log("Marker.on(popup_open)", event);
                let targetMarker = event.target as StationMarker;
                selectedStation = targetMarker;
                console.log("Marker: ", targetMarker);
                console.log("Marker station-id: ", targetMarker.id);

                // Fetch return stations
                let returnStationsResp = await fetch(
                    `${env.PUBLIC_BACKEND_API}/stations/${targetMarker.id}/depatures/returnstations`,
                );
                let returnStations: Returnstation[] =
                    await returnStationsResp.json();
                console.log("# Returnstations: ", returnStations.length);

                // Remove all markers except the one selected
                allMarkers.eachLayer((layer) => {
                    let returnStation = returnStations.find(
                        (r) => r.return_station === (layer as StationMarker).id,
                    );
                    if (layer === targetMarker || returnStation) {
                        markers.addLayer(layer);
                        visibleReturnStations.push([
                            layer as StationMarker,
                            Number(returnStation?.count),
                        ]);
                    } else {
                        markers.removeLayer(layer);
                    }
                });
                // Have to re-assign the array to 'force' reactivity on it
                visibleReturnStations = visibleReturnStations;
                // Draw lines between them
                map?.addLayer(lines);
                returnStations.forEach((returnStation) => {
                    createPolyLine(targetMarker, returnStation).addTo(lines);
                });
                map?.flyToBounds(lines.getBounds(), flyToOptions);

                console.log(markers.getLayers().length);
                console.log(allMarkers.getLayers().length);
            });

            marker.on("popupclose", (event) => {
                console.log("Marker.on(popup_close)", event);
                selectedStation = undefined;
                visibleReturnStations = [];
                lines.clearLayers();
                allMarkers.eachLayer((layer) => {
                    if (layer === event.target) {
                    } else {
                        markers.addLayer(layer);
                    }
                });
                map?.flyToBounds(markers.getBounds(), flyToOptions);
            });
        });
        map?.addLayer(markers);
        map?.flyToBounds(markers.getBounds(), flyToOptions);
    });
    // Search/filter station function
    $: {
        console.log("search--val: " + searchTerm);
        // To stop overriding opacity when clicking a filtered marker
        allMarkers?.eachLayer((m) => {
            if (
                // !(m as StationMarker).options.title
                !(m as StationMarker).station_name
                    ?.toLowerCase()
                    .startsWith(searchTerm.toLowerCase())
            ) {
                if (searchTerm) {
                    selectedStation?.closePopup();
                    markers.removeLayer(m);
                    lines.clearLayers();
                }
            } else {
                markers.addLayer(m);
            }
        });
        console.log(markers.getLayers().length);
        console.log(allMarkers.getLayers().length);
        markers = markers;
    }
    // When searching, reassign markers array to trigger reactivity for resultlist
    // This is not the way to do it *but it works, ok*
</script>

<svelte:head>
    <title>Map</title>
    <meta name="description" content="Map" />
</svelte:head>

<div id="content">
    <div id="sidebar">
        <div id="search">
            <Search
                bind:searchVar={searchTerm}
                placeholder="Search stations..."
            />
            <!-- <br style="clear:both;" /> -->
        </div>

        <div id="stationList">
            {#if selectedStation}
                <ReturnList
                    returnStations={sortByNrOfTrips(visibleReturnStations)}
                    station={selectedStation}
                />
            {:else if searchTerm}
                <SearchResults selection={castFGToArr(markers)} />
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
        width: 20%;
        overflow: auto;
        padding-left: 15px;
        padding-right: 15px;
        padding-top: 15px;
        padding-bottom: 30px;
    }
    #search {
        width: 100%;
        display: flex;
    }
    #map {
        width: 80%;
        height: 100%;
    }
</style>
