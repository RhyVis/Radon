<script setup lang="ts">
import SideMenuEntry from "@/layout/particial/SideMenuEntry.vue";
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { RouteRecordRaw } from "vue-router";

const { name, nameKey, iconName, records } = defineProps<{
  name: string;
  nameKey?: string;
  iconName: string;
  records: RouteRecordRaw[];
}>();

const { t } = useI18n();
const display = computed(() => (nameKey ? t(nameKey) : name));
</script>

<template>
  <t-submenu :value="name.trim().toLowerCase()">
    <template #icon>
      <t-icon :name="iconName" />
    </template>
    <template #title>
      <span>{{ display }}</span>
    </template>
    <SideMenuEntry v-for="(record, index) in records" :record="record" :key="index" />
  </t-submenu>
</template>

<style scoped></style>
