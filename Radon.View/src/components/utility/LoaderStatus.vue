<script lang="ts" setup>
import type { UseLoader } from "@/composable/useLoader.ts";
import { get } from "@vueuse/core";
import { RefreshIcon } from "tdesign-icons-vue-next";
import { computed, inject, type InjectionKey } from "vue";

const { name, label, loaderKey } = defineProps<{
  name: string;
  label: string;
  loaderKey: InjectionKey<UseLoader>;
}>();

const loader = inject(loaderKey)!;
const { hasError, completed, current, restore } = loader;

const id = computed(() => `loader-${name}`);
const theme = computed(() => {
  if (get(hasError)) return "danger";
  if (get(completed)) return "success";
  return "default";
});
const tag = computed(() => {
  if (!get(completed)) return `${label}: ${get(current)}`;
  return label;
});
const icon = computed(() => {
  if (get(hasError)) return "error-circle";
  if (!get(completed)) return "help-circle";
  return "check-circle";
});
</script>

<template>
  <div class="w-fit" :id="id">
    <t-loading :delay="100" :loading="!completed" size="small">
      <t-space :size="4">
        <t-tag :theme="theme" variant="light">
          <template #icon>
            <t-icon :name="icon" />
          </template>
          <span class="r-no-select">{{ tag }}</span>
        </t-tag>
        <t-button v-if="hasError" size="small" theme="danger" variant="dashed" @click="restore">
          <RefreshIcon />
        </t-button>
      </t-space>
    </t-loading>
  </div>
</template>
