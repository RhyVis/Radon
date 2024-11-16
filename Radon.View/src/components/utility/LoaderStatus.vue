<script lang="tsx" setup>
import { CheckCircleIcon, HelpCircleIcon } from "tdesign-icons-vue-next";
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

const id = computed(() => `loading-tag-${name}`);
const theme = computed(() => {
  if (hasError) return "danger";
  if (completed) return "success";
  return "default";
});
const tag = computed(() => `${label}: ${current}`);
</script>

<template>
  <div class="r-ls-div" :id="id">
    <t-loading size="small" :delay="100" :loading="!completed">
      <t-tag :theme="theme" variant="light">
        <template #icon>
          <HelpCircleIcon v-if="!completed" />
          <CheckCircleIcon v-else />
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
