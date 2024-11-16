import type { useDark } from "@vueuse/core";
import type { InjectionKey } from "vue";

export const darkModeKey: InjectionKey<ReturnType<typeof useDark>> = Symbol("darkMode");
