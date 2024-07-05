<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, {
        FeatureGroup,
        LayerGroup,
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
    let selectVal: string;
    // This could be done using StationMarker[] as well, but LG can be used as controller in the future
    const markers: FeatureGroup = new FeatureGroup();
    const lines: FeatureGroup = new FeatureGroup();
    const marker_station_map: { [layer_id: number]: number } = {}; // För att hitta rätt ?

    interface Returnstation {
        return_station: number;
        count: number;
    }

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
            marker.bindPopup(popup);
            markers.addLayer(marker);

            // Instead of event handlers we could set some state in these, ex: singleView=true & selectedMarker = n
            // And react to them using $
            marker.on("popupopen", async (event) => {
                console.log("Marker.on(popup_open)", event);
                let targetMarker = event.target as StationMarker;
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
                    let stationId = (layer as StationMarker).stationId;
                    let returnStation = returnStations.find(
                        (r) => r.return_station === stationId,
                    );
                    if (!(layer === targetMarker || returnStation)) {
                        map?.removeLayer(layer);
                    }
                });
                // Draw lines between them
                returnStations.forEach((returnStation) => {
                    createPolyLine(targetMarker, returnStation).addTo(lines);
                    // Lägg till dom i en featuregroup eller nåt så vi enkelt kan ta bort
                    // https://github.com/IvanSanchez/Leaflet.Polyline.SnakeAnim
                    // Kan lägga till en punkt i mitten så den blir lite ovan eller under, så linjen blir "bågig"
                });
                map?.addLayer(lines);
                map?.fitBounds(lines.getBounds())
                console.log("done");
            });

            marker.on("popupclose", (event) => {
                console.log("Marker.on(popup_close)", event);
                markers.eachLayer((layer) => {
                    if (layer === event.target) {
                    } else {
                        map?.addLayer(layer);
                    }
                });
                lines.clearLayers();
                map?.fitBounds(markers.getBounds())
            });
        });
        map?.addLayer(markers);
        map?.fitBounds(markers.getBounds());
    });
    $: {
        console.log("input--val: " + inpVal);
    }
    $: console.log("select--val: " + selectVal);
</script>

<svelte:head>
    <title>Map</title>
    <meta name="description" content="Map" />
</svelte:head>

<div id="content">
    <div id="sidebar">
        <div id="menu">
            <button on:click={() => map?.flyTo(initPos, 10)}>Reset zoom</button>
        </div>
        <form>
            <select bind:value={selectVal}>
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
            <input bind:value={inpVal} placeholder="Search station" />
            <input bind:value={inpVal} placeholder="Has return station" />
            <p>{inpVal}</p>
        </form>
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
        min-width: 100px;
    }
    #map {
        width: 100%;
        height: 100%;
    }
</style>
