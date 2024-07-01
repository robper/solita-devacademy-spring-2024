<script lang="ts">
    import { onMount, onDestroy, setContext } from "svelte";
    import L from "leaflet";
    import "leaflet/dist/leaflet.css";

    export let bounds: L.LatLngBoundsExpression | undefined = undefined;
    // Initial camerapoint
    export let view: L.LatLngExpression | undefined = [59.3308025, 18.0573745]; 
    // Initial zoom
    export let zoom: number | undefined = 7;
    // Layers to present on the map
    export let layers: L.Layer | undefined = undefined;

    let map: L.Map | undefined;
    let mapElement: HTMLDivElement;

    onMount(() => {
        if (!bounds && (!view || !zoom)) {
            throw new Error("no bounds");
        }
        map = L.map(mapElement);
        L.tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
            attribution: "",
        }).addTo(map);

        // Adds all requested layers (markers etc) to the map, after it's created
        layers?.addTo(map)
    });

    onDestroy(() => {
        map?.remove();
        map = undefined;
    });

    setContext("map", {
        getMap: () => map,
    });

    $: if (map) {
        if (bounds) {
            map.fitBounds(bounds);
        } else if (view && zoom){
            map.setView(view, zoom);
        }
    }
</script>

<div class="map" bind:this={mapElement}>
    {#if map}
        <slot />
    {/if}
</div>
<style>
    .map{
        width: 80%;
        height: 100vh;
    }
</style>