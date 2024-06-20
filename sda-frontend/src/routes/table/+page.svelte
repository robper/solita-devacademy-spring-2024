<script lang="ts">
    //import { Station } from "$lib/types.js"
    export let data;
    console.log(data.stations)
    
    //let stations: Station[] = data.stations;
    console.log('x', data.stations)

    const journeyTable = [
        { column: "id", description: "id, primary key", type: "integer" },
        {
            column: "departure_date_time",
            description: "Journey start timestamp",
            type: "timestamp *NULL*",
        },
        {
            column: "return_date_time",
            description: "Journey end timestamp",
            type: "timestamp *NULL*",
        },
        {
            column: "departure_station_id",
            description: "Journey start station",
            type: "integer, references to station(id)",
        },
        {
            column: "return_station_id",
            description: "Journey end station",
            type: "integer, references to station(id)",
        },
        {
            column: "distance",
            description: "Distance of journey in meters",
            type: "integer *NULL*",
        },
        {
            column: "duration",
            description: "Duration of journey in seconds",
            type: "integer *NULL*",
        },
    ];
</script>

<h1>Station Table</h1>
<table>
    <thead>
        <tr>
            <th>Id</th>
            <th>Station Address</th>
            <th>Station Name</th>
            <th>Coordinate X</th>
            <th>Coordinate Y</th>
        </tr>
    </thead>
    <tbody>
        {#each data.stations as row}
            <tr>
                <td>{row.id}</td>
                <td>{row.station_address}</td>
                <td>{row.station_name}</td>
                <td>{row.coordinate_x}</td>
                <td>{row.coordinate_y}</td>
            </tr>
        {/each}
    </tbody>
</table>

<h1>Journey Table</h1>
<table>
    <thead>
        <tr>
            <th>Column</th>
            <th>Description</th>
            <th>Type</th>
        </tr>
    </thead>
    <tbody>
        {#each journeyTable as row}
            <tr>
                <td>{row.column}</td>
                <td>{row.description}</td>
                <td>{row.type}</td>
            </tr>
        {/each}
    </tbody>
</table>
