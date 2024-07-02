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

    onMount(() => {
        let tempLayers: L.Layer[] = [];
        data.stations.forEach((s) =>
            tempLayers.push(
                L.marker([Number(s.coordinate_y), Number(s.coordinate_x)], {
                    alt: s.station_name ?? "",
                    title: s.station_name ?? "",
                }).on({
                    click: handleMarkerClick,
                }),
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
            map?.setZoom(12);
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
</script>

<svelte:head>
    <title>Map</title>
    <meta name="description" content="Map" />
</svelte:head>

<button on:click={() => map?.flyTo(initPos, 10)}>Reset zoom</button>

<button on:click={() => (markers = [...markers, L.marker([51.64, 7.54])])}>
    Inline {markers.length}
</button>
<Map view={initPos} zoom={10} bind:map />

<!-- <Map view={initPos} zoom={10} {layers} bind:map /> -->

<style></style>
