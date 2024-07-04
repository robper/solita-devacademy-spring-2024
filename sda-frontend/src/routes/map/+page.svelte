<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, { Layer, LayerGroup, Marker, type LatLngExpression } from "leaflet";
    import { onMount } from "svelte";
    export let data;
    import { env } from "$env/dynamic/public";

    let initPos: LatLngExpression = [
        Number(data.stations[0].coordinate_y),
        Number(data.stations[0].coordinate_x),
    ];
    let map: L.Map | undefined;
    let inpval = "";
    let selectval: string;
    const layers: LayerGroup = new LayerGroup();
    const marker_station_map: { [layer_id: number]: number } = {};

    interface Returnstation {
        return_station: number,
        count: number
    }
    onMount(() => {
        data.stations.forEach((s) => {
            
            let m = new Marker([Number(s.coordinate_y), Number(s.coordinate_x)], {
                alt: s.station_name ?? "",
                title: s.station_name ?? "",
            });

            let p = L.popup({}, m).setContent(
                    "<p><a href=/stations/" +
                    s.id +
                    ">" +
                    s.station_name +
                    "</a></p><p>" +
                    s.station_address +
                    "</p>",
            );
            m.bindPopup(p);
            layers.addLayer(m);
            layers.eachLayer((layer) => map?.addLayer(layer));
            marker_station_map[layers.getLayerId(m)] = s.id;

            // Instead of event handlers we could set some state in these, ex: singleView=true & selectedMarker = n
            // And react to them using $
            m.on("popupopen", async (event) => {
                console.log("Marker.on(popup_open)");
                console.log(event);
                // Translate the markers id to its station
                let layer_id = layers.getLayerId(event.target);
                let station_f_layerid = marker_station_map[layer_id];
                console.log("Station id", station_f_layerid);

                map?.flyTo(event.target.getLatLng(), 15);
                
                
                // Fetch return stations
                let ret_stations_resp = await fetch(
                    `${env.PUBLIC_BACKEND_API}/stations/${station_f_layerid}/depatures/returnstations`,
                );
                let ret_stations: Returnstation[] = await ret_stations_resp.json();
                console.log("Returnstations: ", ret_stations);
                console.log("ANTAL Returnstations: ", ret_stations.length);
                
                // Remove all markers except selected
                layers.eachLayer((layer) => {
                    let s_id = marker_station_map[layers.getLayerId(layer)]
                    let ret_station = ret_stations.find( (r) => r.return_station === s_id)
                    //console.log('ret_station', ret_station)
                    if (layer === event.target || ret_station) {
                        console.log('keep', s_id, ret_station?.return_station)
                    } else {
                        map?.removeLayer(layer);
                    }
                });
                // Draw them
                console.log("done");
            });

            m.on("popupclose", (event) => {
                console.log("Marker.on(popup_open)");
                console.log(event);
                layers.eachLayer((layer) => {
                    if (layer === event.target) {
                    } else {
                        map?.addLayer(layer);
                    }
                });
                map?.setZoom(11);
            });
        });
        map?.addLayer(layers);
    });
    $: {
        console.log("input--val: " + inpval);
    }
    $: console.log("select--val: " + selectval);
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
            <select bind:value={selectval}>
                <option>1</option>
                <option>2</option>
                <option>3</option>
            </select>
            <input bind:value={inpval} placeholder="Search" />
            <p>{inpval}</p>
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
