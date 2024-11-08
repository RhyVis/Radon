<script setup lang="ts">
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { intToRoman } from "@/pages/math/roman/scripts/romanNum";

const input = ref(0);
const roman = computed(() => intToRoman(input.value));
</script>

<template>
  <ContentLayout title="罗马数字" subtitle="古典计数法">
    <t-form>
      <t-form-item label="阿拉伯数字">
        <t-input v-model="input" type="number" />
      </t-form-item>
      <t-form-item label="罗马数字">
        <t-textarea :value="roman" :readonly="true" :autosize="true" />
      </t-form-item>
      <t-form-item label="操作">
        <t-space :size="5">
          <ButtonCopy :target="roman" />
        </t-space>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>
