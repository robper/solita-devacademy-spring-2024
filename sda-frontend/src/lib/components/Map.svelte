<script lang="ts">
    import { onMount, onDestroy, setContext } from "svelte";
    import L from "leaflet";
    import "leaflet/dist/leaflet.css";

    export let bounds: L.LatLngBoundsExpression | undefined = undefined;
    // Initial camerapoint
    export let view: L.LatLngExpression = [59.3308025, 18.0573745];
    // Initial zoom
    export let zoom: number | undefined = 7;
    // Layers to present on the map
    export let layers: L.Layer[] | undefined = undefined;
    // Export the map so we can modify it in the referencing component
    export let map: L.Map | undefined;
    let mapElement: HTMLDivElement;

    onMount(() => {
        if (!bounds && (!view || !zoom)) {
            throw new Error("no bounds");
        }
        map = L.map(mapElement).setView(view, zoom);
        L.tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
            attribution: "",
        }).addTo(map);
    });

    onDestroy(() => {
        map?.remove();
        map = undefined;
    });

    setContext("map", {
        getMap: () => map,
    });

    $: if (map) {
        layers?.forEach((f) => f.removeFrom(map as L.Map));
        layers?.forEach((f) => f.addTo(map as L.Map));
    }
</script>

<div class="map" bind:this={mapElement}>
    {#if map}
        <slot />
    {/if}
</div>

<style>
    .map {
        width: auto;
        height: 100%;
    }
</style>
