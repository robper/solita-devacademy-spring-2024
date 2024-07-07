<script lang="ts">
    import { StationMarker } from "$lib/leafletTypes";
    export let station: StationMarker;
    export let returnStations: [station: StationMarker, nrOfTrips: Number][] =
        [];
</script>

<ul>
    <h2>
        <a href="/stations/{station.stationId}">
            {station.options.title}
        </a>
    </h2>
    <p>
        Has trips ending at {returnStations.length} stations
    </p>
    <table>
        <th id="stationCol">Station</th>
        <th id="returnsCol">Nr of trips</th>

        {#each returnStations as returnStation}
            <tr>
                <td id="stationCol">
                    <a href="/stations/{returnStation[0].stationId}">
                        {returnStation[0].options.title}
                    </a>
                </td>
                <td id="returnsCol">
                    {returnStation[1]}
                </td>
            </tr>
        {/each}
    </table>
</ul>

<style>
    ul {
        list-style-type: none;
        padding: 0px;
        margin: 0px;
    }
    ul li {
        padding-bottom: 3px;
    }
    table {
        table-layout: fixed;
        width: 100%;
    }
    #returnsCol {
        text-align: right;
        text-overflow: ellipsis;
        margin: 0;
        padding: 0;
    }
    #stationCol {
        margin: 0;
        padding: 0;
        text-align: left;
        text-overflow: ellipsis;
        width: 60%;
        white-space: nowrap;
        overflow: hidden;
    }
</style>
