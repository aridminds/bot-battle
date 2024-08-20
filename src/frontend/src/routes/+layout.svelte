<script>
	import { onMount } from 'svelte';
	import '../app.css';
	import { userStore } from '$lib/stores/user-store';
	import { getApiPath } from '$lib/base-path';

	onMount(() => {
		fetch(`${getApiPath()}/account/me`, {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json'
			},
			credentials: 'include'
		})
			.then((res) => {
				if (res.ok) {
					return res.json();
				} else {
				}
			})
			.then((data) => {
				userStore.set(data);
			})
			.catch((err) => {});
	});
</script>

<slot />
