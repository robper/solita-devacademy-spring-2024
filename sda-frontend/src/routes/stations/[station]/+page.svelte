<script lang="ts">
    export let data;
    console.log(data.journeys)

</script>
<h1>Journey Table</h1>
<table>
    <thead>
        <tr>
            <th>Id</th>
            <th>departure_date_time</th>
            <th>return_date_time</th>
            <th>departure_station_id</th>
            <th>return_station_id</th>
            <th>distance</th>
            <th>duration</th>
        </tr>
    </thead>
    <tbody>
        {#each data.journeys as journey}
            <tr>
                <td>{journey.id}</td>
                <td>{journey.departure_date_time}</td>
                <td>{journey.return_date_time}</td>
                <td>{journey.departure_station_id}</td>
                <td>{journey.return_station_id}</td>
                <td>{journey.distance}</td>
                <td>{journey.duration}</td>
            </tr>
        {/each}
    </tbody>
</table>
