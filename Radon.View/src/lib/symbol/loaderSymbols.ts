import type { UseLoader } from "@/composable/useLoader";
import type { InjectionKey } from "vue";

export const fontLoaderKey: InjectionKey<UseLoader> = Symbol("fontLoader");
