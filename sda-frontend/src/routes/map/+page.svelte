<script lang="ts">
    import Map from '$lib/components/Map.svelte';
    import L from "leaflet";
    export let data;
    
    let layers: L.Layer = L.marker([51.6, 7.5]);
    let initPos: L.LatLngExpression = [51.514244, 7.468429]
</script>

<svelte:head>
    <title>MapLibre GL JS</title>
    <meta name="description" content="MapLibre GL JS" />
</svelte:head>

<Map view={initPos} zoom={10} layers={layers}/>

<style></style>
