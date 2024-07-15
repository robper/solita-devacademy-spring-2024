<script lang="ts">
    export let selection: Station[];
</script>

<h2 id="top">Stations</h2>
<ul>
    {#each selection as station}
        <li>
            <a href="/stations/{station.id}" target="_blank">
                <p class="left">{station.station_name}</p>
            </a>
        </li>
    {/each}
</ul>

<style>
    ul {
        list-style: none;
        padding: 0;
        margin: 0;
    }
    li {
        display: flex;
        padding-left: 0px;
        padding-right: 0px;
        padding-top: 0px;
        margin: 0px;
        border-bottom: 1px solid var(--color-shade);
    }
    li a {
        width: 100%;
    }
    ul p {
        padding: 0;
        margin-bottom: 5px;
        margin-top: 5px;
    }
    .left {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
    }
</style>
