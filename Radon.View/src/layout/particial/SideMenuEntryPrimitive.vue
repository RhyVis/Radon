<script lang="ts" setup>
import { useGlobalStore } from "@/store/global.ts";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const {
  auth = false,
  titleKey,
  icon,
  value,
  to,
} = defineProps<{
  auth?: boolean;
  titleKey?: string;
  icon?: string;
  value: string;
  to: string;
}>();

const { t } = useI18n();
const { push } = useRouter();
const { authPassed, sideVisible } = storeToRefs(useGlobalStore());

const canShowRecord = computed(() => !(auth && !get(authPassed)));
const display = computed(() => (titleKey ? t(titleKey) : value));

const handleClick = () => {
  set(sideVisible, false);
  push(to);
};
</script>

<template>
  <t-menu-item v-if="canShowRecord" :value="value" @click="handleClick">
    <template #icon>
      <t-icon v-if="icon" :name="icon" />
      <slot v-else name="icon" />
    </template>
    <span>{{ display }}</span>
    <slot />
  </t-menu-item>
</template>
