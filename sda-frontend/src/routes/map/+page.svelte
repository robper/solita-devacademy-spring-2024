<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, {
        LayerGroup,
        Polyline,
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
    const layers: LayerGroup = new LayerGroup();
    const marker_station_map: { [layer_id: number]: number } = {}; // För att hitta rätt ?

    interface Returnstation {
        return_station: number;
        count: number;
    }

    onMount(() => {
        data.stations.forEach((s) => {
            let marker = new StationMarker(
                [Number(s.coordinate_y), Number(s.coordinate_x)],
                {
                    alt: s.station_name ?? "",
                    title: s.station_name ?? "",
                },
            ).setStationId(s.id);

            let p = L.popup({}, marker).setContent(
                "<p><a href=/stations/" +
                    s.id +
                    ">" +
                    s.station_name +
                    " " +
                    s.id +
                    "</a></p><p>" +
                    s.station_address +
                    "</p>",
            );
            marker.bindPopup(p);
            layers.addLayer(marker);

            // Instead of event handlers we could set some state in these, ex: singleView=true & selectedMarker = n
            // And react to them using $
            marker.on("popupopen", async (event) => {
                console.log("Marker.on(popup_open)", event);
                let marker = event.target as StationMarker;
                console.log("Marker: ", marker);
                console.log("Marker station-id: ", marker.stationId);

                map?.flyTo(marker.getLatLng(), 15);

                // Fetch return stations
                let returnStationsResp = await fetch(
                    `${env.PUBLIC_BACKEND_API}/stations/${marker.stationId}/depatures/returnstations`,
                );
                let returnStations: Returnstation[] =
                    await returnStationsResp.json();
                console.log("# Returnstations: ", returnStations.length);

                // Remove all markers except the one selected
                layers.eachLayer((layer) => {
                    let stationId = (layer as StationMarker).stationId;
                    let returnStation = returnStations.find(
                        (r) => r.return_station === stationId,
                    );
                    if (layer === marker || returnStation) {
                        //console.log("keep", s_id, ret_station?.return_station);
                    } else {
                        map?.removeLayer(layer);
                    }
                });
                // Draw lines between them
                returnStations.forEach((rs) => {
                    // Find the the marker which belongs to the return station
                    let returnStationMarker = layers
                        .getLayers()
                        .find(
                            (layer) =>
                                (layer as StationMarker).stationId ===
                                rs.return_station,
                        ) as StationMarker;
                    let mLat = marker.getLatLng().lat;
                    let mLng = marker.getLatLng().lng;
                    let rLat = returnStationMarker.getLatLng().lat;
                    let rLng = returnStationMarker.getLatLng().lng;
                    let line = new Polyline(
                        [
                            [mLat, mLng],
                            [rLat, rLng],
                        ],
                        { color: "red" },
                    ).addTo(map);
                    // Lägg till dom i en featuregroup eller nåt så vi enkelt kan ta bort
                    // https://github.com/IvanSanchez/Leaflet.Polyline.SnakeAnim
                    // Kan lägga till en punkt i mitten så den blir lite ovan eller under, så linjen blir "bågig"
                    //map?.addLayer(line);
                });
                console.log("done");
            });

            marker.on("popupclose", (event) => {
                console.log("Marker.on(popup_close)", event);
                layers.eachLayer((layer) => {
                    if (layer === event.target) {
                    } else {
                        map?.addLayer(layer);
                    }
                });
                map?.setZoom(10);
            });
        });
        map?.addLayer(layers);
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
        <p>{layers.getLayers().length}</p>
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
