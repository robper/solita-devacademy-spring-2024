<script lang="ts">
    export let data;
    console.log(data.stations)

</script>

<h1>Station Table</h1>
<table>
    <thead>
        <tr>
            <th>Id</th>
            <th>Station Name</th>
            <th>Station Address</th>
            <th>Coordinate X</th>
            <th>Coordinate Y</th>
        </tr>
    </thead>
    <tbody>
        {#each data.stations as station}
            <tr>
                <td><a href="stations/{station.id.toString()}">{station.id}</a></td>
                <td><a href="stations/{station.id.toString()}">{station.station_name}</a></td>
                <td>{station.station_address}</td>
                <td>{station.coordinate_x}</td>
                <td>{station.coordinate_y}</td>
            </tr>
        {/each}
    </tbody>
</table>