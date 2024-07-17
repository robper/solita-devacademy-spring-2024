<script lang="ts">
    export let selection: Station[];
</script>

<h2>Stations</h2>
<ul>
    {#each selection as station}
        <li>
            <a href="/stations/{station.id}" target="_blank">
                <p>{station.station_name}</p>
            </a>
        </li>
    {/each}
</ul>

<style>
    ul {
        list-style: none;
        margin: 0;
        padding: 0;
    }
    li {
        margin: 0px -0.5rem 1px -0.5rem;
        padding: 0;
    }
    li a {
        display: block;
        border-radius: 5px;
        margin: 0;
        padding: 0.25rem 0.5rem;
        text-decoration: none;
        width: 100%;
    }
    p {
        padding: 0;
        margin: 0;
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
    }
    li a:hover {
        background-color: var(--color-popout);
        transition-duration: 0.1s;
    }
</style>
