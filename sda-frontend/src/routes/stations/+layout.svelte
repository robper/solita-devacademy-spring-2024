<script lang="ts">
    import { page } from "$app/stores";

    export let data;
    let path: string | undefined;

    $: path = $page.url.pathname;
</script>

<div id="stations-layout">
    <div id="stations-list">
        <h2 id="top">Stations</h2>
        <ul>
            {#each data.stations as station}
                <li
                    class={path === `/stations/${station.id}`
                        ? "active"
                        : undefined}
                >
                    <a href="/stations/{station.id}">
                        {station.station_name}
                    </a>
                </li>
            {/each}
        </ul>
    </div>

    <div id="single-station">
        <slot />
    </div>
</div>

<style>
    #stations-layout {
        display: flex;
        flex-direction: row;
        justify-content: flex-start;
    }
    #stations-list {
        min-width: 250px;
        width: 15%;
        height: calc(100vh - 55px);
        overflow: scroll;
        padding: 0rem 1rem 2rem 1rem;
        border-right: 1px solid var(--color-shade);
    }
    #single-station {
        padding-left: 15px;
        width: 85%;
        min-width: 250px;
        background-color: var(--color-foreground);
    }
    ul {
        margin: 0;
        list-style: none;
        padding: 0;
    }
    li {
        padding: 0;
        margin: 0px -0.5rem 1px -0.5rem;
    }
    li a {
        border-radius: 5px;
        display: block;
        margin: 0;
        padding: 0.25rem 0.5rem;
        text-decoration: none;
        width: 100%;
    }
    li a:hover {
        background-color: var(--color-popout);
        text-decoration: none;
        transition-duration: 0.1s;
    }
    li.active a {
        background-color: var(--color-shade);
    }
</style>
