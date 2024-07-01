<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L from "leaflet";
    import { onMount } from "svelte";
    export let data;
    let layers: L.Layer[] = [L.marker([51.6, 7.5])];
    let initPos: L.LatLngExpression = [51.514244, 7.468429];
    let map: L.Map | undefined;
    onMount(() => {
        let tempLayers: L.Layer[] = [];
        data.stations.forEach((s) =>
            tempLayers.push(
                L.marker([Number(s.coordinate_y), Number(s.coordinate_x)]).on(
                    "click",
                    () => console.log("click"),
                    // Toggla något state som tar bort alla 
                    // utom den som klickades på
                    // Sen rita linjer från journeys
                ),
            ),
        );
        layers = layers.concat(tempLayers);
        console.log(layers);
    });

    function flyto(){
        map?.flyTo(initPos, 13)
    }
    function addLayer() {
        layers = [...layers, L.marker([51.64, 7.54])];
    }
    initPos = [
        Number(data.stations[0].coordinate_y),
        Number(data.stations[0].coordinate_x),
    ] as L.LatLngExpression;
</script>

<svelte:head>
    <title>MapLibre GL JS</title>
    <meta name="description" content="MapLibre GL JS" />
</svelte:head>

<button on:click={() => addLayer()}>Function {layers.length}</button>
<button on:click={() => flyto()}>FlyTo</button>

<button on:click={() => (layers = [...layers, L.marker([51.64, 7.54])])}>
    Inline {layers.length}
</button>
<Map view={initPos} zoom={10} {layers} bind:map={map} />

<style></style>
