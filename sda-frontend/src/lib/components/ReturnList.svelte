<script lang="ts">
    import { StationMarker } from "$lib/leafletTypes";
    export let station: StationMarker;
    export let returnStations: [station: StationMarker, nrOfTrips: Number][] =
        [];
</script>

<body>
    <h2 id="top">
        <a href="/stations/{station.id}" target="_blank">
            {station.options.title}
        </a>
    </h2>
    <p>
        Has trips ending at {returnStations.length} stations
    </p>
    <section id="tableheader">
        <p>Station</p>
        <p># Trips</p>
    </section>
    <ul>
        {#each returnStations as returnStation}
            <li>
                <a href="/stations/{returnStation[0].id}" target="_blank">
                    <p class="left">{returnStation[0].station_name}</p>
                    <p>{returnStation[1]}</p>
                </a>
            </li>
        {/each}
    </ul>
</body>

<style>
    #tableheader {
        display: flex;
        justify-content: space-between;
        padding-left: 0px;
        padding-right: 0px;
        padding-top: 0px;
        margin: 0px;
        border-bottom: 1px solid var(--color-black);
    }
    ul {
        list-style: none;
        padding: 0;
        margin: 0;
        /* border-top: 1px solid var(--color-gray); */
        margin-bottom: 30px;
    }
    li {
        display: flex;
        justify-content: space-between;
        padding-left: 0px;
        padding-right: 0px;
        padding-top: 0px;
        margin: 0px;
        border-bottom: 1px solid var(--color-gray);
        /* border-left: 1px solid var(--color-gray); */
        /* border-right: 1px solid var(--color-gray); */
    }
    li:hover {
        background-color: var(--color-secondary);
    }
    li a {
        width: 100%;
        display: flex;
        justify-content: space-between;
        gap: 10px;
    }
    p {
        padding: 0;
        margin-bottom: 5px;
        margin-top: 5px;
    }

    a {
        text-decoration: none;
        color: var(--color-black);
    }
    a:hover {
        color: var(--color-gray);
        border-radius: var(--border-radius);
        text-decoration: underline;
    }
    .left {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
    }
</style>
