import { useWindowSize } from "@vueuse/core";
import { computed } from "vue";

export const useNarrow = () => {
  const { width } = useWindowSize();
  return computed(() => width.value < 768);
};
