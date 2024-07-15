<script lang="ts">
    import { preloadData, pushState, goto } from "$app/navigation";
    import { page } from "$app/stores";
    import StationPage from "./[station]/+page.svelte";

    export let data;
</script>

<div id="stations">
    <div id="sr">
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
                        <p>{station.station_name}</p>
                    </a>
                </li>
            {/each}
        </ul>
    </div>
    <div id="single">
        {#if $page.state.selected}
            <StationPage data={$page.state.selected} />
        {/if}
    </div>
</div>

<style>
    #stations {
        display: flex;
        flex-direction: row;
        justify-content: flex-start;
    }
    #sr {
        min-width: 250px;
        width: 20%;
        height: calc(100vh - 55px);
        overflow: scroll;
        padding-left: 15px;
        padding-right: 15px;
        padding-bottom: 30px;
        border-right: 1px solid var(--color-shade);
    }
    #single {
        padding-left: 15px;
        width: 80%;
        min-width: 250px;
        background-color: var(--color-foreground);
    }
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
</style>
