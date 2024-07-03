<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L from "leaflet";
    import { onMount } from "svelte";
    export let data;

    let markers: L.Layer[] = [];
    let initPos: L.LatLngExpression = [
        Number(data.stations[0].coordinate_y),
        Number(data.stations[0].coordinate_x),
    ];
    let map: L.Map | undefined;
    let singleView = false;
    let inpval = "";
    let selectval: string;
    onMount(() => {
        let tempLayers: L.Layer[] = [];
        data.stations.forEach((s) =>
            tempLayers.push(
                L.marker([Number(s.coordinate_y), Number(s.coordinate_x)], {
                    alt: s.station_name ?? "",
                    title: s.station_name ?? "",
                })
                    .on({
                        click: handleMarkerClick,
                    })
                    .bindPopup(
                        "<p><a href=/stations/" +
                            s.id +
                            ">" +
                            s.station_name +
                            "</a></p><p>" +
                            s.station_address +
                            "</p>",
                    ),
            ),
        );
        markers = markers.concat(tempLayers);
        markers.forEach((layer) => map?.addLayer(layer));
    });

    function handleMarkerClick(event: L.LeafletMouseEvent) {
        let marker = event.target as L.Marker;
        if (singleView) {
            // Re-add all markers and zoom out a bit
            markers.forEach((layer) => map?.addLayer(layer));
            map?.setZoom(11);
        } else {
            // Pan to selected marker
            map?.flyTo(marker.getLatLng(), 15);
            // Remove all markers
            markers.forEach((lay) => map?.removeLayer(lay));
            // Re-add selected marker
            map?.addLayer(event.target);
        }
        singleView = !singleView;
    }
    $: {
        console.log("input--val: " + inpval);
    }
    $: console.log("select--val: " + selectval);
    // Kan göra en groupby i backen och bara skicka ut vilka som som faktiskt går mellan
    // Sen typ en siffra på linen som säger hur många. Ja
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
