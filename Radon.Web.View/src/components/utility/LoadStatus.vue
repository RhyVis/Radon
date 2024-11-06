<script lang="tsx" setup>
import { CheckCircleIcon, HelpCircleIcon } from "tdesign-icons-vue-next";

const props = defineProps<{
  name: string;
  label: string;
  loading: boolean;
}>();
const id = computed(() => `loading-tag-${props.name}`);
const theme = computed(() => (props.loading ? "default" : "success"));
</script>

<template>
  <div class="r-ls-div" :id="id">
    <t-loading size="small" :delay="100" :loading="loading">
      <t-tag :theme="theme" variant="light">
        <template #icon>
          <HelpCircleIcon v-if="loading" />
          <CheckCircleIcon v-else />
        </template>
        <span class="r-no-select">{{ label }}</span>
      </t-tag>
    </t-loading>
  </div>
</template>

<style scoped>
.r-ls-div {
  width: fit-content;
}
</style>
