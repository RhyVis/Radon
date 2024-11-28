<script setup lang="ts">
import { storeToRefs } from "pinia";

import { useGlobalStore } from "@/store/global.ts";
import { get, set } from "@vueuse/core";
import type { MenuRoute, MenuValue } from "tdesign-vue-next";
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { RouteRecordRaw } from "vue-router";

const { record } = defineProps<{
  record: RouteRecordRaw;
}>();

const { t } = useI18n();
const { authPassed, asideVisible } = storeToRefs(useGlobalStore());
const canShowRecord = computed(() => !((record.meta?.auth ?? false) && !get(authPassed)));
const display = computed(() => (record.meta?.tKey ? t(record.meta.tKey) : record.meta!.title));
const handleClose = () => set(asideVisible, false);
</script>

<template>
  <t-menu-item
    v-if="canShowRecord"
    :to="record.path as MenuRoute"
    :value="record.name as MenuValue"
    @click="handleClose"
  >
    <template #icon>
      <t-icon v-if="record.meta?.icon" :name="record.meta.icon" />
    </template>
    <span>{{ display }}</span>
  </t-menu-item>
</template>
