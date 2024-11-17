<script lang="ts" setup>
import { computed } from "vue";

const {
  name,
  label,
  current,
  completed,
  hasError = false,
} = defineProps<{
  name: string;
  label: string;
  current: string;
  completed: boolean;
  hasError?: boolean;
}>();

const id = computed(() => `loader-${name}`);
const theme = computed(() => {
  if (hasError) return "danger";
  if (completed) return "success";
  return "default";
});
const tag = computed(() => {
  if (!completed) return `${label}: ${current}`;
  return label;
});
const icon = computed(() => {
  if (hasError) return "error-circle";
  if (!completed) return "help-circle";
  return "check-circle";
});
</script>

<template>
  <div class="r-ls-div" :id="id">
    <t-loading size="small" :delay="100" :loading="!completed">
      <t-tag :theme="theme" variant="light" size="small">
        <template #icon>
          <t-icon :name="icon" />
        </template>
        <span class="r-no-select">{{ tag }}</span>
      </t-tag>
    </t-loading>
  </div>
</template>

<style scoped>
.r-ls-div {
  width: fit-content;
}
</style>
