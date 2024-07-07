<script lang="ts">
    import { onMount, onDestroy, setContext } from "svelte";
    import L from "leaflet";
    import "leaflet/dist/leaflet.css";

    export let bounds: L.LatLngBoundsExpression | undefined = undefined;
    // Initial camerapoint
    export let view: L.LatLngExpression = [59.3308025, 18.0573745];
    // Initial zoom
    export let zoom: number | undefined = 7;
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

    // Context allows us to have multiple instances of this component
    setContext("map", {
        getMap: () => map,
    });

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
