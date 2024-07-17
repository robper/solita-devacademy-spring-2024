<script lang="ts">
    import { preloadData, pushState, goto } from "$app/navigation";
    import { page } from "$app/stores";
    import StationPage from "./[station]/+page.svelte";

    export let data;
</script>

<svelte:head>
    <title>Stations</title>
    <meta name="description" content="Stations" />
</svelte:head>

<div id="content">
    <div id="stations-list">
        <h2 id="top">Stations</h2>
        <ul>
            {#each data.stations as station}
                <li>
                    <a
                        href="/stations/{station.id}"
                        on:click={async (e) => {
                            if (
                                innerWidth < 640 || // bail if the screen is too small
                                e.shiftKey || // or the link is opened in a new window
                                e.metaKey ||
                                e.ctrlKey // or a new tab (mac: metaKey, win/linux: ctrlKey)
                                // should also consider clicking with a mouse scroll wheel
                            )
                                return;

                            // prevent navigation
                            e.preventDefault();

                            const { href } = e.currentTarget;

                            // run `load` functions (or rather, get the result of the `load` functions
                            // that are already running because of `data-sveltekit-preload-data`)
                            const result = await preloadData(href);

                            if (
                                result.type === "loaded" &&
                                result.status === 200
                            ) {
                                /* pushState("", { selected: result.data }); */
                                pushState("", {
                                    selected: {
                                        station: result.data.station,
                                        depatures_count:
                                            result.data.depatures_count,
                                        depatures_distance:
                                            result.data.depatures_distance,
                                        depatures_duration:
                                            result.data.depatures_duration,
                                        returns_count:
                                            result.data.returns_count,
                                    },
                                });
                            } else {
                                // something bad happened! try navigating
                                goto(href);
                            }
                        }}
                    >
                        <!-- <p>{station.station_name}</p> -->
                        {station.station_name}
                    </a>
                </li>
            {/each}
        </ul>
    </div>
    <div id="single-station">
        {#if $page.state.selected}
            <StationPage data={$page.state.selected} />
        {/if}
    </div>
</div>

<style>
    #content {
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
    }
</style>
