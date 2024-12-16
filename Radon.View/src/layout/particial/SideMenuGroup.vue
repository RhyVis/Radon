<script lang="ts" setup>
import SideMenuEntry from "@/layout/particial/SideMenuEntry.vue";
import SideMenuSub from "@/layout/particial/SideMenuSub.vue";
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
const value = computed(() => name.replace(/\s+/g, "").toLowerCase());
const display = computed(() => (nameKey ? t(nameKey) : name));
</script>

<template>
  <SideMenuSub :icon="iconName" :value="value">
    <template #title>
      <span>{{ display }}</span>
    </template>
    <SideMenuEntry v-for="(record, index) in records" :key="index" :record="record" />
    <slot />
  </SideMenuSub>
</template>
