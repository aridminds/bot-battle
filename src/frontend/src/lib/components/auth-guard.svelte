<script lang="ts">
	import { userStore } from '$lib/stores/user-store';
</script>

{#if $userStore?.username}
	<slot name="authorized" />
{:else}
	<slot name="unauthorized" />
{/if}
