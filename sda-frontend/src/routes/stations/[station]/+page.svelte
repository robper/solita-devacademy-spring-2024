<script lang="ts">
    export let data;
</script>
<h1>Station {data.station.station_name}</h1>
<p>Address <a target="_blank" href="https://www.google.se/maps/place/{data.station.station_address}">{data.station.station_address}</a></p>
<p>Coordinates <a href="https://www.google.se/maps/@{data.station.coordinate_y},{data.station.coordinate_x},16.25z">
    ({data.station.coordinate_y}, {data.station.coordinate_x})</a></p>
<h2>Journeys</h2>
<p>{data.depatures_count} starting from here with an avg distance of ~{Math.round(data.depatures_distance)}m
     and an avg duration of ~{Math.round(data.depatures_duration)}s</p>
<p>{data.returns_count} ending here</p>