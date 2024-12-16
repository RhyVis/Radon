<script lang="ts" setup>
import SideMenuEntryPrimitive from "@/layout/particial/SideMenuEntryPrimitive.vue";
import { computed } from "vue";
import type { RouteRecordRaw } from "vue-router";

const { record } = defineProps<{
  record: RouteRecordRaw;
}>();
const { name, path, meta } = record;
const { tKey, icon } = meta ?? {};

const value = computed(() => name?.toString() ?? `unset-${Math.random()}`);
const auth = computed(() => record.meta?.auth ?? false);
</script>

<template>
  <SideMenuEntryPrimitive :auth="auth" :icon="icon" :title-key="tKey" :to="path" :value="value" />
</template>
