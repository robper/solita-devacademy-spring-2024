<script lang="ts">
	import { page } from '$app/stores';
</script>

<h1>This is an error page for all errors on all routes</h1>
<h2>Here's your error</h2>
<p>{$page.error?.message}</p>