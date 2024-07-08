<script lang="ts">
    import type { StationMarker } from "$lib/leafletTypes";

    export let selection: StationMarker[];
</script>

    <h2 id="top">
        Stations
    </h2>
    <ul>
        {#each selection as station}
            <li>
                <a href="/stations/{station.stationId}" target="_blank">
                    <p class="left">{station.stationName}</p>
                </a>
            </li>
        {/each}
    </ul>

<style>
    h2 {
        color: var(--color-black);
    }
    ul {
        list-style: none;
        padding: 0;
        margin: 0;
        margin-bottom: 30px;
    }
    li {
        display: flex;
        justify-content: space-between;
        padding-left: 5px;
        padding-right: 5px;
        padding-top: 0px;
        margin: 0px;
        border-bottom: 1px solid var(--color-gray);
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
    ul p {
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
