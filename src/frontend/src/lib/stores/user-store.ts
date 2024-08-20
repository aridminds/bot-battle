import { writable } from 'svelte/store';

export const userStore = writable<{
	username: string;
} | null>(null);
