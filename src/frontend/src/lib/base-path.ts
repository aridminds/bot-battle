import { dev } from '$app/environment';
import { base } from '$app/paths';

export const getApiPath = (): string => {
	// if (dev) return '';

	return window?.location.origin + base + '/api';
};

export const getAppPath = (): string => {
	return window?.location.origin + base;
};
