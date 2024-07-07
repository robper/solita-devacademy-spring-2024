<script lang="ts">
    export let searchVar: string;
    export let placeholder: string = "Search...";
</script>

<input id="stationSearch" bind:value={searchVar} {placeholder} />

<style>
    input {
        width: inherit;
        height: 20px;
        border-radius: 10px;
        padding: 10px;
        background-color: #e6e6e6;
        outline: none;
        border: 1px solid;
        border-color: #e6e6e6;
        background-image: url("/github-mark.png");
        background-position: 7px, 0px;
        background-size: 20px 20px;
        background-repeat: no-repeat;
        padding-left: 35px;
    }
    input:focus {
        border: 1px solid #8f8f8f;
        transition: 0.5s;
    }
</style>
