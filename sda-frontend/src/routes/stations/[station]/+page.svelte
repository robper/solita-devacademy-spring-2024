<script lang="ts">
    import Map from "$lib/components/Map.svelte";
    import L, { Marker, type LatLngExpression } from "leaflet";

    export let data;

    let initPos: LatLngExpression = [
        Number(data.station.coordinate_y),
        Number(data.station.coordinate_x),
    ];

    let map: L.Map | undefined;

    $: {
        initPos = [
            Number(data.station.coordinate_y),
            Number(data.station.coordinate_x),
        ];
        map?.addLayer(new Marker(initPos));
    }
</script>

<svelte:head>
    <title>{data.station.station_name}</title>
    <meta name="description" content={data.station.station_name} />
</svelte:head>

<h2>
    {data.station.station_name}
</h2>
<p>
    Address
    <a
        target="_blank"
        href="https://www.google.se/maps/place/{data.station.station_address}"
    >
        {data.station.station_address}
    </a>
</p>
<div id="map">
    <!-- Force recreation of the map when selected station changes -->
    {#key data.station}
        <Map view={initPos} zoom={13} bind:map />
    {/key}
</div>
<h3>Journeys</h3>
<p>
    {data.depatures_count} starting from here with an avg. distance of ~{Math.round(
        data.depatures_distance,
    )}m and an avg. duration of ~{Math.round(data.depatures_duration)}s
</p>
<p>{data.returns_count} ending here</p>

<style>
    #map {
        width: 300px;
        height: 300px;
    }
    a:hover {
        background-color: var(--color-foreground);
    }
</style>
