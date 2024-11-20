import { set } from "@vueuse/core";
import { computed, type MaybeRef, ref, toValue } from "vue";

export const useStatus = (initial?: MaybeRef<boolean | number>) => {
  const statusType = ref(
    toValue(() => {
      if (initial === undefined) return 0;
      if (typeof initial === "number") return initial;
      if (typeof initial === "boolean") return initial ? 1 : -1;
      const value = toValue(initial);
      if (typeof value === "number") return value;
      return value ? 1 : -1;
    }),
  );

  const status = computed(() => {
    if (statusType.value === 0) return "default";
    if (statusType.value > 1) return "success";
    if (statusType.value < 0) return "error";
    return "warning";
  });

  const setStatus = (value: boolean | number | undefined) => {
    if (value === undefined) {
      set(statusType, 0);
    }
    if (typeof value === "boolean") {
      set(statusType, value ? 1 : -1);
    }
    if (typeof value === "number") {
      set(statusType, value);
    }
    console.error("Invalid status value", value);
    set(statusType, -2);
  };

  return [status, setStatus];
};
