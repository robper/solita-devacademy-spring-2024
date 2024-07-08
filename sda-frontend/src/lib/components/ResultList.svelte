<script lang="ts">
    import type { StationMarker } from "$lib/leafletTypes";

    export let selection: StationMarker[];
</script>
{#each selection as s}
   <p>{s.options.title}</p> 
{/each}