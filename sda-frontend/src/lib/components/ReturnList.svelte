<script lang="ts">
    import { StationMarker } from "$lib/leafletTypes";
    export let station: StationMarker;
    export let returnStations: [station: StationMarker, nrOfTrips: Number][] =
        [];
</script>

<h2>
    <a href="/stations/{station.id}" target="_blank">
        {station.options.title}
    </a>
</h2>
<p>
    Has trips ending at {returnStations.length} stations
</p>
<section id="list-header">
    <p class="left">Station <span class="right"># Trips</span></p>
</section>
<ul>
    {#each returnStations as returnStation}
        <li>
            <a href="/stations/{returnStation[0].id}" target="_blank">
                <p class="left">
                    {returnStation[0].station_name}
                    <span class="right">{returnStation[1]}</span>
                </p>
            </a>
        </li>
    {/each}
</ul>

<style>
    #list-header {
        display: block;
        /* Top Right Bot Left */
        margin: 1rem 0 0.5rem 0;
        padding: 0;
        border-bottom: 1px solid var(--color-text);
    }
    h2 {
        margin-bottom: 0.5rem;
        overflow-wrap: break-word;
    }
    ul {
        margin: 0;
        list-style: none;
        padding: 0;
    }
    li {
        margin: 0px -0.5rem 1px -0.5rem;
        padding: 0;
    }
    li a {
        border-radius: 5px;
        display: block;
        justify-content: space-between;
        padding: 0.25rem 0.5rem;
        width: 100%;
    }
    a {
        text-decoration: none;
    }
    p {
        padding: 0;
        margin: 0;
    }
    .left {
        overflow: hidden;
        text-overflow: ellipsis;
        text-align: start;
        white-space: nowrap;
    }
    .right {
        float: right;
    }
    li a:hover {
        background-color: var(--color-popout);
        transition-duration: 0.1s;
    }
</style>
