<script lang="ts">
    export let searchVar: string;
    export let placeholder: string = "Search...";
</script>

<input id="stationSearch" bind:value={searchVar} {placeholder} />

<style>
    input {
        width: inherit;
        /* height: 20px; */
        border-radius: 10px;
        background-color: var(--color-secondary);
        outline: none;
        border: 1px solid;
        border-color: var(--color-secondary);
        background-image: url("/search.png");
        background-position: 7px, 0px;
        background-size: 18px 18px;
        background-repeat: no-repeat;
        padding-left: 35px;
        padding-right: 10px;
        padding-top: 10px;
        padding-bottom: 10px;
    }
    input:focus {
        border: 1px solid var(--color-gray);
        transition: 0.5s;
    }
</style>
