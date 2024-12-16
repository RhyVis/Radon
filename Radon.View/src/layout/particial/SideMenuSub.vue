<script lang="ts" setup>
import { useI18n } from "vue-i18n";

const { value, titleKey, icon } = defineProps<{
  value: string;
  titleKey?: string;
  icon?: string;
}>();

const { t } = useI18n();
</script>

<template>
  <t-submenu :value="value">
    <template #icon>
      <t-icon v-if="icon" :name="icon" />
      <slot v-else name="icon" />
    </template>
    <template #title>
      <span v-if="titleKey">{{ t(titleKey) }}</span>
      <slot v-else name="title" />
    </template>
    <slot />
  </t-submenu>
</template>
