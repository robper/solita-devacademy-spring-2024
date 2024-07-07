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
		background-color: #f5f5f5;
		height: calc(100vh - 55px);
		width: 100vw;
	}
	nav {
		height: 55px;
		width: 100%;
		background-color: #ffffff;
		display: flex;
		flex-direction: row;
		justify-content: flex-start;
		gap: 10px;
		align-items: center;
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
		border-radius: 50px;
		/* border: 3px solid; */
		padding: 10px;
		font-size: 1rem;
		text-decoration: none;
		color: black;
		background: white;
		display: block;
	}
	a:hover {
		background-color: #f1f1f1;
	}
	img {
		height: 25px;
	}
</style>
