import { ref } from "vue";

export const useKeyUpdate = () => {
  const key = ref(0);
  const updateKey = () => {
    key.value++;
  };
  return { updateKey, key };
};
