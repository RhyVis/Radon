import { useLoader } from "@/composable/useLoader";
import type { InjectionKey } from "vue";

export const fontLoaderKey: InjectionKey<ReturnType<typeof useLoader>> = Symbol("fontLoader");
