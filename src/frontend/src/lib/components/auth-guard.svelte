<script lang="ts">
	import { userStore } from '$lib/stores/user-store';
</script>

{#if $userStore?.username}
	<slot />
{/if}
