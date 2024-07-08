<nav>
	<div class="left">
		<a href="/">Home</a>
		<a href="/stations">Stations</a>
		<a href="/map">Map</a>
	</div>
	<div class="right">
		<a class="icon" href="https://github.com" target="_blank">
			<img src="/github-mark.png" alt="git" />
		</a>
	</div>
</nav>
<div id="content">
	<slot />
</div>

<style>
	#content {
		background-color: var(--color-primary);
		height: calc(100vh - 55px);
		width: 100vw;
		z-index: 1;
		position: relative;
	}
	nav {
		height: 55px;
		width: 100%;
		background-color: var(--color-white);
		display: flex;
		flex-direction: row;
		justify-content: flex-start;

		/* Shadow*/
		z-index: 2;
		position: relative;
		align-items: center;
		box-shadow: 0px 0px 4px 1px var(--color-gray);
	}
	.right {
		margin-left: auto;
		margin-right: 10px;
	}
	.left {
		display: flex;
		justify-content: flex-start;
		gap: 5px;
		margin-left: 10px;
	}
	a {
		border-radius: 10px;
		padding: 10px;
		font-size: 1rem;
		text-decoration: none;
		color: black;
		display: block;
	}
	a:hover {
		background-color: var(--color-secondary);
	}
	/* Git logo*/
	img {
		height: 25px;
	}
</style>
